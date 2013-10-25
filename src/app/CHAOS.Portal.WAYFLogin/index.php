<?php
	require_once('../../../tools/simplesamlphp-1.11.0/lib/_autoload.php');
	$API_PATH = "https://myapi.com";
?>
<html>
    <head>
        <title>Wayf Login Test</title>
    </head>
    <body>
        <h1>Wayf Login Test</h1>
        <p>
		<?php	   
			$simpleSaml = new SimpleSAML_Auth_Simple("LARM.fm SP");
		   
			if(!isset($_REQUEST["sessionGuid"]))
				print("Parameter sessionGuid not set");
			else
			{
				$simpleSaml->requireAuth();
				
				$sessionGuid = $_REQUEST["sessionGuid"];
				$attributes = $simpleSaml->getAttributes();
				
				$parameters = array(
					'format' => 'json2',
					'sessionGuid' => $sessionGuid,
					'wayfId' => $attributes['eduPersonTargetedID'][0],
					'givenName' => $attributes['GivenName'][0],
					'surName' => $attributes['SurName'][0],
					'commonName' => $attributes['CommonName'][0],
					);
				
				$ch = curl_init();

				curl_setopt($ch, CURLOPT_URL, $API_PATH + "/v6/Wayf/Login");
				curl_setopt($ch, CURLOPT_POST, 1);
				curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($parameters));
				curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

				$rawResponse = curl_exec ($ch);

				curl_close ($ch);
				
				$response = json_decode($rawResponse);
				
				if($response == null)
					print("Failed to parse Portal response");
				else
				{
					if($response["Error"]["Message"] != null)
						print("Portal authentication failed: " + $response["Error"]["Message"]);
					
					print("Session authenticated");
				}
			}
		?>
        </p>
    </body>
</html>