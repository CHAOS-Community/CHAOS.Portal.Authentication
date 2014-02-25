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