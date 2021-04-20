<?php 
function RegisterMenu()
{
	$data[]=array(
		'position' => 'Header',
	);
		
	return $data;
}
function RegisterPages()
{
	$pages['home_page']=array(
		'page_name' => 'Home Page',
		'page_url' => url('/'),
		'file_location' => '/views/welcome/customize.php',
	);
	return $pages;
}

function RegisterThemeOption()
{
	$data[]=array(
		'title'=>'Theme Option',
		'url'=>url('/'),
	);

	return $data;
}

function RegisterSitemap()
{
	$data[]= array(
		'url' => url('/').'/page/', 
		'type' => 1, 
	);
	$data[]= array(
		'url' => url('/').'/blog/', 
		'type' => 0, 
	);
	

	$static[]= array(
		'url' => url('/').'/blog', 	
	);
	

	$arr = array( 
		'dynamic' => $data, 
		'static' => $static, 
	);
	return $arr;
	

}



 ?>