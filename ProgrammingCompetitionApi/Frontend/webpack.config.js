const WebpackBuildNotifierPlugin = require('webpack-build-notifier');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const path = require('path');

var output_path = path.resolve(__dirname, '../wwwroot');

const plugins = [
	new MiniCssExtractPlugin({
		filename: '../css/app.css'
	}),
	new CleanWebpackPlugin({
		cleanOnceBeforeBuildPatterns: [
			output_path + '/css',
			output_path + '/js',
		],
        dangerouslyAllowCleanPatternsOutsideProject: true
	}),
	new WebpackBuildNotifierPlugin({
		suppressSuccess: true,
	}),
	{
		apply(compiler) {
			if (compiler.options.watch)
				compiler.hooks.done.tap('timeInfo', () => {
					console.log(('[' + new Date().toLocaleString() + ']') + ' Compilation finished');
				});
		}
	}
];

var rules = [
{
	test: /\.(js|jsx)$/i,
	use: {
		loader: "babel-loader",
		options: {
			presets: [
				'@babel/preset-env', 
				["@babel/preset-react", {"runtime": "automatic"}]
			]
		}
	}
},
{
	test: /\.css$/i,
	use: [
		MiniCssExtractPlugin.loader,
		'css-loader',
	],
},
{
	test: /\.s[ac]ss$/i,
	use: [
		MiniCssExtractPlugin.loader,
		'css-loader',// Translates CSS into CommonJS
		{
			loader: "sass-loader",// Compiles Sass to CSS
			options: {
				sassOptions: {
					indentType: 'tab',
					indentWidth: 1,
					outputStyle: 'expanded',
				},
			},
		}
	],
},
{
	test: /\.(bmp|jpg|jpeg|png)$/i,
	use: [
		{
			loader: 'file-loader',
			options: {
				name: '[name].[ext]',
				outputPath: '/images/',
			}
		}
	],
},
{
	test: /\.(woff(2)?|ttf|eot)(\?v=\d+\.\d+\.\d+)?$/,
	use: [
		{
			loader: 'file-loader',
			options: {
				name: '[name].[ext]',
				outputPath: '/fonts/'
			}
		}
	]
}];

const config = {
	entry: './src/Index.js',
	output: {
		filename: 'app.js',
		path: output_path + '/js/',
		publicPath: '/../',
	},
	stats: 'errors-warnings',
	module: {
		rules
	},
	plugins
};

module.exports = (env, argv) => {
	if (argv.mode === 'development') {
		config.devtool = 'source-map';
	}
	else if (argv.mode === 'production') {
		
	}

	return config;
};