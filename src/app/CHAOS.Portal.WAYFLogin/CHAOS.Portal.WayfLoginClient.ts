/// <reference path="TypeScriptDefinitions/PortalClient.d.ts" />

module CHAOS.Portal
{
	export class WayfLoginClient
	{
		public static Login(client: CHAOS.Portal.Client.IPortalClient, wayfServicePath:string,frame:HTMLIFrameElement, callback:(success:boolean) => void): void
		{
			if (!client.SessionAcquired())
				throw "IPortalClient does not have a session";

			if (wayfServicePath.substr(wayfServicePath.length - 1, 1) != "/")
				wayfServicePath += "/";

			if (callback != null)
			{
				var messageRecieved = (event:MessageEvent) =>
				{
					window.removeEventListener("message", messageRecieved, false);
					callback(event.data == "success");
				};

				window.addEventListener("message", messageRecieved, false);
			}

			frame.src = wayfServicePath + "?sessionGuid=" + client.GetCurrentSession().Guid + "&apiPath=" + client.GetServicePath();
		}
	}
}