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
		// 	target: "https://localhost:1253",
		// },
		"/signalRHub": {
			target: "https://localhost:1253",
			ws: true,
			secure: false,
		},
		"/": {
			secure: false,
			target: "https://localhost:1253",
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
