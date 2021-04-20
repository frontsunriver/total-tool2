<?php 


namespace Amcoders\Plugin;

/**
 * 
 */
class Plugin 
{
	public static function is_active($name)
	{
		$file = file_get_contents( base_path().'/am-content/Plugins/plugin.json');
		$plugins = json_decode($file, true);
		foreach ($plugins as $key => $value) {
			if ($value['Text Domain'] == $name) {
				if ($value['status'] == 'active') {
					return true;
				}
			}
		}
	}
}