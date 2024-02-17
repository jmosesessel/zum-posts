const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7037';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target,
    secure: false
  },
  //{
  //  context: [
  //    "https://localhost:7037/api/posts",
  //  ],
  //  target,
  //  secure: false
  //}
]

module.exports = PROXY_CONFIG;
