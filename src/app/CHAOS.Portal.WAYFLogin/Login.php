<?php
	ini_set('display_errors',1);
	ini_set('display_startup_errors',1);
	error_reporting(-1);

	require_once('../../simplesamlphp/lib/_autoload.php');
	require_once('WayfConfiguration.php');
	
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
		$simpleSaml->requireAuth();
		$attributes = $simpleSaml->getAttributes();
		
		$helper = new PortalHelper($_REQUEST["apiPath"]);
		
		$response = $helper->Call("AuthKey/Login", array('token' => $WayfConfiguration['AuthKeyToken']));
		
		if(!($error = $helper->GetError()))
		{
			$helper->SetSessionGuid($response->Body->Results[0]->Guid);
			$response = $helper->Call("Wayf/Login", array('wayfId' => $attributes['eduPersonTargetedID'][0], 'email' => $attributes['mail'][0], 'sessionGuidToAuthenticate' => $_REQUEST["sessionGuid"]));
		
			if(!($error = $helper->GetError()))
			{
				//$response = $helper->Call("UserManagement/GetUserObject", array('createIfMissing ' => true));

				$error = $helper->GetError();
			}
		}
		
		if(isset($_REQUEST["callbackUrl"]))
		{
			$status = ($error == null ? "success" : "failure");
			
			/*if(isset(http_build_url))
				header('Location: ' . http_build_url($_REQUEST["callbackUrl"], array("query" => "status=" . $status), HTTP_URL_JOIN_QUERY), true, 303);
			else*/
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
				print("Session authenticated");
			else
				print("Error: $error");
		?>
        </p>
    </body>
</html>

<?php
	class PortalHelper
	{
		private $_sessionGuid;
		private $_path;
		private $_ch;

		private $_error;
		
		function __construct($path)
		{
			$this->_path = $path;
			
			$this->_ch = curl_init();
		}
		
		function __destruct()
		{
			curl_close ($this->_ch);
		}
		
		public function SetSessionGuid($sessionGuid)
		{
			$this->_sessionGuid = $sessionGuid;
		}
	
		public function Call($path, $parameters)
		{
			$parameters['format'] = 'json2';
			
			if($this->_sessionGuid)
				$parameters['sessionGUID'] = $this->_sessionGuid;
			
			$this->_error = null;
		
			curl_setopt($this->_ch, CURLOPT_URL, "$this->_path/v6/$path");
			curl_setopt($this->_ch, CURLOPT_POST, 1);
			curl_setopt($this->_ch, CURLOPT_POSTFIELDS, http_build_query($parameters));
			curl_setopt($this->_ch, CURLOPT_RETURNTRANSFER, true);
			
			$rawResponse = curl_exec ($this->_ch);
			
			$response = json_decode($rawResponse);
			
			if($response == null)
				$this->_error = "Failed to parse Portal response. Raw data was: $rawResponse";
			else if ($response->Error != null && $response->Error->Message != null)
				$this->_error = "Portal authentication failed: " . $response->Error->Message;
				
			return $response;
		}
		
		public function GetError()
		{
			return $this->_error;
		}
	}
?>