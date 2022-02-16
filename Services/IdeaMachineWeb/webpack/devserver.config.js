const path = require("path");
const fs = require("fs");

exports.cfg = {
	static: {
		directory: path.join(__dirname, "wwwroot"),
	},
	https: {
		key: fs.readFileSync(path.join(__dirname, "./certs/devserverhttps.pem")),
		cert: fs.readFileSync(path.join(__dirname, "./certs/devserverhttps.crt")),
	},
	host: "localhost",
	port: 21534,
	proxy: {
		// "/signalRHub/negotiate": {
		// 	secure: false,
		// 	target: "https://localhost:1457",
		// },
		// "/signalRHub": {
		// 	target: "ws://localhost:1457/signalRHub",
		// 	ws: true,
		// },
		"/": {
			secure: false,
			ws: true,
			target: "https://localhost:1457",
		},
	},
	client: {
		overlay: true,
		webSocketTransport: "ws",
	},
	webSocketServer: "ws",
	hot: true,
	historyApiFallback: {
		index: "index.html",
	},
	devMiddleware: {
		publicPath: "/dist/",
		writeToDisk: true,
	},
};
