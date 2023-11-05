const PORT = 3004;
const HyperExpress = require('hyper-express');
const webserver = new HyperExpress.Server();

webserver.get('/', (request, response) => {
    response.send('OK');
})

webserver.listen(PORT)
.then((socket) => console.log(`Webserver started on port ${PORT}`))
.catch((error) => console.log(`Failed to start webserver on port ${PORT}`, error));
