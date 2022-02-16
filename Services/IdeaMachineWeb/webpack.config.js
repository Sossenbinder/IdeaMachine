const path = require("path");
const statements = require("tsx-control-statements").default;
const keysTransformer = require("ts-transformer-keys/transformer").default;
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const devServerConfig = require("./webpack/devserver.config.js");

const config = {
	devServer: devServerConfig.cfg,
	mode: "development",
	entry: "./src/index.tsx",
	output: {
		path: path.resolve(__dirname, "wwwroot/dist"),
		filename: "site.js",
	},
	devtool: "source-map",
	module: {
		rules: [
			{
				test: /\.module.scss$/,
				use: [
					"style-loader",
					{
						loader: "css-loader",
						options: {
							modules: {
								localIdentName: "[local]_[hash:base64:5]",
							},
							url: false,
						},
					},
					"postcss-loader",
					"sass-loader",
				],
			},
			{
				test: /\.scss$/,
				exclude: /\.module\.scss$/,
				use: ["style-loader", "css-loader", "postcss-loader", "sass-loader"],
			},
			{
				test: /\.tsx?$/,
				loader: "ts-loader",
				exclude: /node_modules/,
				options: {
					getCustomTransformers: (program) => ({
						before: [statements(), keysTransformer(program)],
					}),
				},
			},
		],
	},
	resolve: {
		extensions: [".tsx", ".ts", ".jsx", ".js", ".css", ".less"],
		plugins: [new TsconfigPathsPlugin({ configFile: "./tsconfig.json" })],
		modules: ["node_modules", "src/Common/"],
	},
};

module.exports = config;
