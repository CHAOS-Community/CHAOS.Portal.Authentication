<?php
	require_once('../../../tools/simplesamlphp-1.11.0/lib/_autoload.php');
	
	$error = null;
	
	if(!isset($_REQUEST["apiPath"]))
		print("Parameter apiPath not set");
	if(!isset($_REQUEST["sessionGuid"]))
		print("Parameter sessionGuid not set");
	else
	{
		//$simpleSaml = new SimpleSAML_Auth_Simple("Wayf");
			
		//$simpleSaml->requireAuth();
				
		$sessionGuid = $_REQUEST["sessionGuid"];
		$apiPath = $_REQUEST["apiPath"];
		//$attributes = $simpleSaml->getAttributes();
				
		$attributes = {
						'eduPersonTargetedID' => 'testid',
						'mail' => 'test@test.test',
						'gn' => 'Jens',
						'sn' => 'Jensen'
		};
				
		$parameters = array(
			'format' => 'json2',
			'sessionGuid' => $sessionGuid,
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
			$error = "Failed to parse Portal response";
		else if ($response["Error"] != null && $response["Error"]["Message"] != null)
			$error = "Portal authentication failed: " + $response["Error"]["Message"];
?>
<html>
    <head>
        <title>Wayf Login</title>
		<script type="text/javascript">
			location.hash = "status=<?php print($error == null ? 'success' : 'failure'); ?>";
		</script>
    </head>
    <body>
        <h1>Wayf Login</h1>
        <p>
		<?php
			if($error == null)
				print("Session authenticated");
			else
				print("Error: $error")
		?>
        </p>
    </body>
</html>