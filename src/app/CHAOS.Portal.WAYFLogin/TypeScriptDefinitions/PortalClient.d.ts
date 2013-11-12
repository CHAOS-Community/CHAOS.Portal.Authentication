declare module CHAOS.Portal.Client {
    interface IPortalClient {
        GetServicePath(): string;
        GetCurrentSession(): ISession;
        HasSession(): boolean;
        IsAuthenticated(): boolean;
        SessionAcquired(): IEvent;
        SessionAuthenticated(): IEvent;
        ClientGuid: string;
    }
    interface ISession {
        Guid: string;
        UserGuid: string;
        DateCreated: number;
        DateModified: number;
        FullName: string;
    }
    interface IServiceCaller {
        CallService<T>(path: string, method?: HttpMethod, parameters?: {
            [index: string]: any;
        }, requiresSession?: boolean): ICallState<T>;
        GetServiceCallUri(path: string, parameters: {
            [index: string]: any;
        }, requiresSession: boolean, format: string): string;
        UpdateSession(session: ISession): void;
        SetSessionAuthenticated(type: string, userGuid: string, sessionDateModified: number): void;
    }
    interface ICallState<T> {
        WithCallback(callback: (response: IPortalResponse<T>) => void): ICallState<T>;
        WithCallback(callback: (response: IPortalResponse<T>) => void, context: any): ICallState<T>;
        WithCallbackAndToken(callback: (response: IPortalResponse<T>, token: any) => void, token: any): ICallState<T>;
        WithCallbackAndToken(callback: (response: IPortalResponse<T>, token: any) => void, token: any, context: any): ICallState<T>;
    }
    interface IPortalResponse<T> {
        Header: IHeader;
        Body: IPortalResult<T>;
        Error: IError;
    }
    interface IHeader {
        Duration: number;
    }
    interface IPortalResult<T> {
        Count: number;
        TotalCount: number;
        Results: T[];
    }
    interface IError {
        Fullname: string;
        Message: string;
        Stacktrace: string;
        InnerException: IError;
    }
    interface IEvent {
        Add(handler: (any: any) => void): void;
        Remove(handler: (any: any) => void): void;
    }
    enum HttpMethod {
        Get,
        Post,
    }
}
declare module CHAOS.Portal.Client {
    class PortalClient implements Client.IPortalClient, Client.IServiceCaller {
        static GetClientVersion(): string;
        private static GetProtocolVersion();
        private _servicePath;
        private _currentSession;
        private _authenticationType;
        private _sessionAcquired;
        private _sessionAuthenticated;
        public GetServicePath(): string;
        public GetCurrentSession(): Client.ISession;
        public HasSession(): boolean;
        public IsAuthenticated(): boolean;
        public SessionAcquired(): Client.IEvent;
        public SessionAuthenticated(): Client.IEvent;
        public ClientGuid: string;
        constructor(servicePath: string, clientGuid?: string);
        public CallService<T>(path: string, method?: Client.HttpMethod, parameters?: {
            [index: string]: any;
        }, requiresSession?: boolean): Client.ICallState<T>;
        public GetServiceCallUri(path: string, parameters?: {
            [index: string]: any;
        }, requiresSession?: boolean, format?: string): string;
        private GetPathToExtension(path);
        private AddSessionToParameters(parameters);
        public UpdateSession(session: Client.ISession): void;
        public SetSessionAuthenticated(type: string, userGuid: string, sessionDateModified: number): void;
    }
}
declare module CHAOS.Portal.Client {
    class Session {
        static Create(serviceCaller?: Client.IServiceCaller): Client.ICallState<Client.ISession>;
        static Get(serviceCaller?: Client.IServiceCaller): Client.ICallState<Client.ISession>;
        static Update(serviceCaller?: Client.IServiceCaller): Client.ICallState<Client.ISession>;
        static Delete(serviceCaller?: Client.IServiceCaller): Client.ICallState<Client.ISession>;
    }
    class EmailPassword {
        static AuthenticationType(): string;
        static Login(email: string, password: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static SetPassword(userGuid: string, newPassword: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class SecureCookie {
        static AuthenticationType(): string;
        static Create(serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Login(guid: string, passwordGuid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class User {
        static Create(guid: string, email: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Update(guid: string, email: string, permissons?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(guid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Get(guid?: string, groupGuid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static GetCurrent(serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Group {
        static Get(guid?: string, userGuid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Create(name: string, systemPermission: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Update(guid: string, newName: string, newSystemPermission?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(guid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static AddUser(guid: string, userGuid: string, permissions: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static RemoveUser(guid: string, userGuid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static UpdateUserPermissions(guid: string, userGuid: string, permissions: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class View {
        static Get(view: string, query?: string, sort?: string, filter?: string, pageIndex?: number, pageSize?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static List(serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class ClientSettings {
        static Get(guid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Set(guid: string, name: string, settings: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    function Initialize(servicePath: string, clientGUID?: string, autoCreateSession?: boolean): IPortalClient;
    class ServiceCallerService {
        private static _defaultCaller;
        static GetDefaultCaller(): Client.IServiceCaller;
        static SetDefaultCaller(value: Client.IServiceCaller): void;
    }
}
declare module CHAOS.Portal.Client {
    class MetadataSchema {
        static Get(guid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Create(name: string, schemaXml: string, guid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Update(name: string, schemaXml: string, guid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(guid: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static HasPermissionToMetadataSchema(guid: string, permission: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Folder {
        static GetPermission(folderID: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static SetPermission(userGuid: string, groupGuid: string, folderID: number, permission: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Get(id?: number, folderTypeID?: number, parentID?: number, permission?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(id: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Update(id: number, newTitle: string, newParentID?: number, newFolderTypeID?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Create(subscriptionGuid: string, title: string, parentID?: number, folderTypeID?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class FolderType {
        static Get(name?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Format {
        static Get(name?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class FormatType {
        static Get(name?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Language {
        static Get(name?: string, languageCode?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Object {
        static Create(guid: string, objectTypeID: number, folderID: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Get(objectGuids: string[], accessPointGuid?: string, includeMetadata?: boolean, includeFiles?: boolean, includeObjectRelations?: boolean, includeFolders?: boolean, includeAccessPoints?: boolean, pageSize?: number, pageIndex?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static SetPublishSettings(objectGUID: string, accessPointGUID: string, startDate: Date, endDate: Date, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class ObjectRelation {
        static Set(object1Guid: string, object2Guid: string, objectRelationTypeID: number, sequence?: number, metadataGuid?: string, metadataSchemaGuid?: string, languageCode?: string, metadataXml?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(object1Guid: string, object2Guid: string, objectRelationTypeID: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class ObjectRelationType {
        static Get(value?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class Metadata {
        static Set(objectGuid: string, metadataSchemaGuid: string, languageCode: string, revisionID: number, metadataXml: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class ObjectType {
        static Get(serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Set(name: string, id?: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Delete(id: number, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class UserManagement {
        static GetUserFolder(userGuid?: string, createIfMissing?: boolean, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static GetUserObject(userGuid?: string, createIfMissing?: boolean, includeMetata?: boolean, includeFiles?: boolean, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
    class UserProfile {
        static Get(metadataSchemaGuid: string, userGuid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
        static Set(metadataSchemaGuid: string, metadata: string, userGuid?: string, serviceCaller?: Client.IServiceCaller): Client.ICallState<any>;
    }
}
declare module CHAOS.Portal.Client {
    class SecureCookieHelper {
        private static COOKIE_LIFE_TIME_DAYS;
        static DoesCookieExist(): boolean;
        static Login(callback?: (success: boolean) => void, serviceCaller?: Client.IServiceCaller): void;
        static Create(serviceCaller?: Client.IServiceCaller): void;
        static Clear(): void;
        private static GetCookie();
        private static SetCookie(guid, passwordGuid, expireInDays);
    }
}
