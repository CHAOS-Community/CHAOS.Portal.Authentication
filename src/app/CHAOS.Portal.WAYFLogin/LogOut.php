<?php
	ini_set('display_errors',1);
	ini_set('display_startup_errors',1);
	error_reporting(-1);

	require_once('../../simplesamlphp/lib/_autoload.php');
	require_once('WayfConfiguration.php');
	require_once('PortalHelper.php');
	
	$error = null;
	
	if(!isset($_REQUEST["apiPath"]))
		$error = "Parameter apiPath not set";
	else if(!isset($_REQUEST["sessionGuid"]))
		$error = "Parameter sessionGuid not set";
	else if(!isset($WayfConfiguration['AuthKeyToken']))
		$error = "AuthKeyToken not set in configuration";
	else
	{
		$simpleSaml = new SimpleSAML_Auth_Simple("Wayf");
		$simpleSaml->logout(array(
						 'ReturnCallback' => 'LoggedOut'
					));
	}
	
	function LoggedOut()
	{
		$helper = new PortalHelper($_REQUEST["apiPath"]);
		$helper->SetSessionGuid($_REQUEST["sessionGuid"]);
		$response = $helper->Call("Session/Delete", array('sessionGUID' => $_REQUEST["sessionGuid"]));
		$error = $helper->GetError();
		
		if(isset($_REQUEST["callbackUrl"]))
		{
			$status = ($error == null ? "success" : "failure");
			
			header('Location: ' . $_REQUEST["callbackUrl"] . "?status=" . $status, true, 303);

			exit();
		}
	}
?>

<html>
    <head>
        <title>Wayf Login</title>
		<script type="text/javascript">
			var status = "WayfStatus: <?php print($error == null ? 'success' : 'failure'); ?>";
			
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
			if($error == null)
				print("Session logged out");
			else
				print("Error: $error");
		?>
        </p>
    </body>
</html>