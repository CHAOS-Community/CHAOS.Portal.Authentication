/// <reference path="TypeScriptDefinitions/PortalClient.d.ts" />

module CHAOS
{
	export class WayfLoginClient
	{
		public static Login(client: CHAOS.Portal.Client.IPortalClient, wayfServicePath:string,frame:HTMLIFrameElement, callback:(success:boolean) => void): void
		{
			if (!client.SessionAcquired())
				throw "IPortalClient does not have a session";

			frame.onload = (e: Event) =>
			{
				var statusIndex = frame.contentWindow.location.hash.indexOf("status");

				if (statusIndex == -1) return;

				callback(frame.contentWindow.location.hash.substr(statusIndex + 7, 7) == "success");
			};

			frame.src = wayfServicePath + "CHAOSWayfLogin.php?sessionGuid=" + client.GetCurrentSession().Guid + "&apiPath=" + client.GetServicePath();
		}
	}
}