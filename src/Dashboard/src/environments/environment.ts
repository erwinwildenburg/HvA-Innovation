// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  baseTitle: 'HvA Innovation',
  apiUrl: 'https://api.innovation.hva.powershelldsc.cloud',

  authority: 'https://identity.innovation.hva.powershelldsc.cloud',
  client_id: 'dashboard',
  response_type: 'id_token token',
  scope: 'openid api'
};
