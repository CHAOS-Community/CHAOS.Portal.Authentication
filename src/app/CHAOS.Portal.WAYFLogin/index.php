<?php
	ini_set('display_errors',1);
	ini_set('display_startup_errors',1);
	error_reporting(-1);

	require_once('../../simplesamlphp/lib/_autoload.php');
	
	$error = null;
	
	if(!isset($_REQUEST["apiPath"]))
		$error = "Parameter apiPath not set";
	if(!isset($_REQUEST["sessionGuid"]))
		$error = "Parameter sessionGuid not set";
	else
	{
		//$simpleSaml = new SimpleSAML_Auth_Simple("Wayf");
		//$simpleSaml->requireAuth();
		//$attributes = $simpleSaml->getAttributes();
				
		$attributes = array(
						'eduPersonTargetedID' => 'testid',
						'mail' => 'test@test.test',
						'gn' => 'Jens',
						'sn' => 'Jensen'
					);
		
		$helper = new PortalHelper($_REQUEST["apiPath"], $_REQUEST["sessionGuid"]);

		$response = $helper->Call("Wayf/Login", array('wayfId' => $attributes['eduPersonTargetedID'][0], 'email' => $attributes['mail'][0]));
		
		if(!($error = $helper->GetError()))
		{
			//$response = $helper->Call("UserManagement/GetUserObject", array('createIfMissing ' => true));

			$error = $helper->GetError();
		}
	}
?>

<html>
    <head>
        <title>Wayf Login</title>
		<script type="text/javascript">
			if(parent.window.postMessage)
				parent.window.postMessage("<?php print($error == null ? 'success' : 'failure'); ?>", "*");
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
		
		function __construct($path, $sessionGuid)
		{
			$this->_sessionGuid = $sessionGuid;
			$this->_path = $path;
			
			$this->_ch = curl_init();
		}
		
		function __destruct()
		{
			curl_close ($this->_ch);
		}
	
		public function Call($path, $parameters)
		{
			$parameters['format'] = 'json2';
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