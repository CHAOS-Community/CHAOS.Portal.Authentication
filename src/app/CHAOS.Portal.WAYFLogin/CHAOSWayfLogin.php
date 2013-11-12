<?php
	ini_set('display_errors',1);
	ini_set('display_startup_errors',1);
	error_reporting(-1);

	require_once('../lib/_autoload.php');
	
	$error = null;
	
	if(!isset($_REQUEST["apiPath"]))
		$error = "Parameter apiPath not set";
	if(!isset($_REQUEST["sessionGuid"]))
		$error = "Parameter sessionGuid not set";
	else
	{
		//$simpleSaml = new SimpleSAML_Auth_Simple("Wayf");
			
		//$simpleSaml->requireAuth();
				
		$sessionGuid = $_REQUEST["sessionGuid"];
		$apiPath = $_REQUEST["apiPath"];
		//$attributes = $simpleSaml->getAttributes();
				
		$attributes = array(
						'eduPersonTargetedID' => 'testid',
						'mail' => 'test@test.test',
						'gn' => 'Jens',
						'sn' => 'Jensen'
					);
				
		$parameters = array(
						'format' => 'json2',
						'sessionGUID' => $sessionGuid,
						'wayfId' => $attributes['eduPersonTargetedID'][0],
						'email' => $attributes['mail'][0],
						'givenName' => $attributes['gn'][0],
						'surName' => $attributes['sn'][0]
					);
				
		$ch = curl_init();

		curl_setopt($ch, CURLOPT_URL, "$apiPath/v6/Wayf/Login");
		curl_setopt($ch, CURLOPT_POST, 1);
		curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($parameters));
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

		$rawResponse = curl_exec ($ch);

		curl_close ($ch);
		
		$response = json_decode($rawResponse);
				
		if($response == null)
			$error = "Failed to parse Portal response. Raw data was: $rawResponse";
		else if ($response->Error != null && $response->Error->Message != null)
			$error = "Portal authentication failed: " . $response->Error->Message;
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