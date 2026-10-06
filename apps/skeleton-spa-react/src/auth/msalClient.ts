import { PublicClientApplication, type Configuration } from '@azure/msal-browser'
import { env } from '../env'

const msalConfig: Configuration = {
  auth: {
    clientId: env.entraClientId,
    authority: env.entraAuthority,
    redirectUri: window.location.origin,
    postLogoutRedirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: 'sessionStorage',
    storeAuthStateInCookie: false,
  },
}

export const msalClient = new PublicClientApplication(msalConfig)

export const apiTokenRequest = {
  scopes: [env.apiScope],
}
