<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;

class PluginGenerate extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'make:plugin { name }';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'Create Plugin for Lpress CMS';

    /**
     * Create a new command instance.
     *
     * @return void
     */
    public function __construct()
    {
        parent::__construct();
    }

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public function handle()
    {
        $plugin_domain = $this->argument('name');

        if (!file_exists(base_path().'/am-content/Plugins/'.$plugin_domain)) {
            mkdir(base_path().'/am-content/Plugins/'.$plugin_domain,0777,true);
            $plugin_name = $this->ask('What is plugin name?');
            $plugin_description = $this->ask('What is plugin description?');
            $plugin_author = $this->ask('What is plugin author name?');
            $plugin_author_url = $this->ask('What is plugin author url?');
            $plugin_version = $this->ask('What is plugin version?');
            $this->helper($plugin_domain);
            $this->plugin($plugin_domain,$plugin_name,$plugin_description,$plugin_author,$plugin_author_url,$plugin_version);
            $this->routes($plugin_domain);
            $this->controller($plugin_domain);


            $additionalArray = array(
                "Plugin Name" => $plugin_name,
                "Description" => $plugin_author,
                "Author" => $plugin_author,
                "Author URI" => $plugin_author,
                "Version" => $plugin_version,
                "License" => "GNU General Public License v2 or later",
                "Text Domain" => $plugin_domain,
                "status" => "deactive"
            );

            //open or read json data
            $data_results = file_get_contents( base_path().'/am-content/Plugins/plugin.json');
            $tempArray = json_decode($data_results);

            //append additional json to json file
            $tempArray[] = $additionalArray;
        
            file_put_contents(base_path().'/am-content/Plugins/plugin.json', json_encode($tempArray,JSON_PRETTY_PRINT));  



            $this->info("plugin created successfully.");
        }else{
            $this->info("Plugin already exists!");
        }


    }


    protected function getStub($type)
    { 
        return file_get_contents(app_path("stub/plugin/$type.stub"));
    }


    protected function plugin($plugin_domain,$plugin_name,$plugin_description,$plugin_author,$plugin_author_url,$plugin_version)
    {
        $plugin_generate = str_replace(
            [

                '{{PluginDomain}}',
                '{{PluginDes}}',
                '{{PluginName}}',
                '{{PluginAuthor}}',
                '{{PluginAuthorUrl}}',
                '{{PluginVersion}}'
            ],
            [

                $plugin_domain,
                $plugin_name,
                $plugin_description,
                $plugin_author,
                $plugin_author_url,
                $plugin_version
            ],
            $this->getStub('single_plugin')
        );


        file_put_contents(base_path().'/am-content/Plugins/'.$plugin_domain.'/single_plugin.json', $plugin_generate);
    }

    protected function helper($plugin_domain)
    {
        $plugin_generate = $this->getStub('single_helper');
        file_put_contents(base_path().'/am-content/Plugins/'.$plugin_domain.'/single_helper.json', $plugin_generate);
    }

    protected function routes($plugin_domain)
    {
        $pluginroutes = str_replace(
            ['{{pluginDomain}}'],
            [$plugin_domain],
            $this->getStub('web')
        );
        mkdir(base_path().'/am-content/Plugins/'.$plugin_domain.'/routes',0777,true);
        mkdir(base_path().'/am-content/Plugins/'.$plugin_domain.'/migrations',0777,true);
        mkdir(base_path().'/am-content/Plugins/'.$plugin_domain.'/views',0777,true);

        file_put_contents(base_path().'/am-content/Plugins/'.$plugin_domain.'/routes/web.php', $pluginroutes);
    }

    protected function controller($plugin_domain)
    {
        $modelTemplate = str_replace(
            ['{{pluginDomain}}'],
            [$plugin_domain],
            $this->getStub('controller')
        );
        mkdir(base_path().'/am-content/Plugins/'.$plugin_domain.'/http/controllers',0777,true);

        file_put_contents(base_path().'/am-content/Plugins/'.$plugin_domain.'/http/controllers/TestController.php', $modelTemplate);
    }
}
