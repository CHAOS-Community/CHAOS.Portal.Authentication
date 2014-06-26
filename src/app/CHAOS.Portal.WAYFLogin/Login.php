<?php
	ini_set('display_errors',1);
	ini_set('display_startup_errors',1);
	error_reporting(-1);

	require_once('../../simplesamlphp/lib/_autoload.php');
	require_once('WayfConfiguration.php');
	require_once('PortalHelper.php');
	
	$error = null;
	$status = Login();

	if(isset($_REQUEST["callbackUrl"]))
	{
		header('Location: ' . $_REQUEST["callbackUrl"] . "?status=" . $status . ($error == null ? "" : "&error=" . $error), true, 303);

		exit();
	}

	function Login()
	{
		if(!isset($_REQUEST["apiPath"]))
		{
			ReportError("Parameter apiPath not set");
			return 1;
		}
		if(!isset($_REQUEST["sessionGuid"]))
		{
			ReportError("Parameter sessionGuid not set");
			return 1;
		}
		
		if(!isset($WayfConfiguration['AuthKeyToken']))
		{
			ReportError("AuthKeyToken not set in configuration");
			return 1;
		}
			
		$simpleSaml = new SimpleSAML_Auth_Simple("Wayf");
		$simpleSaml->requireAuth();
		$attributes = $simpleSaml->getAttributes();

		return PortalLogin($attributes);
	}

	function PortalLogin($attributes)
	{
		$encodedAttributes = json_encode($attributes);

		if(!$encodedAttributes)
		{
			$error = "Failed to encode wayf attributes";
			return 1;
		}

		$helper = new PortalHelper($_REQUEST["apiPath"]);
		
		$response = $helper->Call("AuthKey/Login", array('token' => $WayfConfiguration['AuthKeyToken']));

		$error = $helper->GetError();
		if($error != null) return 1;

		$helper->SetSessionGuid($response->Body->Results[0]->Guid);

		$response = $helper->Call("Wayf/Login", array('attributes' => $encodedAttributes, 'sessionGuidToAuthenticate' => $_REQUEST["sessionGuid"]));

		$error = $helper->GetError();
		if($error != null) return 2;
		
		$helper->Call("WayfProfile/Update", array('userGuid' => $response->Body->Results[0]->Guid, 'attributes' => $encodedAttributes));
		$error = $helper->GetError();
		if($error != null) return 1;

		return 0;
	}

	function ReportError($message)
	{
		$error = $message;
		return false;
	}
?>

<html>
    <head>
        <title>Wayf Login</title>
		<script type="text/javascript">
			var status = "WayfStatus: <?php $status; ?>";
			
			if(window.opener && window.opener.postMessage)
				window.opener.postMessage(status, "*");
			else if(window.parent && window.parent !== window && window.parent.window && window.parent.window.postMessage)
				window.parent.window.postMessage(status, "*");
			else if(window.addEventListener)
				window.addEventListener("message", ListenForStatusRequest, false);
			else if(window.attachEvent)
				window.addEventListener("onmessage", ListenForStatusRequest);
			
			function ListenForStatusRequest(event)
			{
				if(event.data == "WayfStatusRequest")
					event.source.postMessage(status, event.origin);
			}
		</script>
    </head>
    <body>
        <h1>Wayf Login</h1>
        <p>
		<?php
			if($status == 0)
				print("Session authenticated");
			else
				print("Error: $error");
		?>
        </p>
    </body>
</html>