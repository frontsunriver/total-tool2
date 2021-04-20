<?php

use Illuminate\Support\Facades\Route;

use App\Http\Controllers\AdminController;
use App\Http\Controllers\PublicPostController;
use App\Http\Controllers\PublicTagController;
use App\Http\Controllers\ProfileController;
use App\Http\Controllers\SettingController;
use App\Http\Controllers\InstantController;
use App\Http\Controllers\FileUploadController;
use App\Http\Controllers\EmbedController;
use App\Http\Controllers\HomeController;
use App\Http\Controllers\TagController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\PageController;
use App\Http\Controllers\PostController;
use App\Http\Controllers\LikeController;
use App\Http\Controllers\Auth\LoginController;

Route::get('admin', [AdminController::class, 'index'])->name('admin');

Route::get('/', [PublicPostController::class, 'index']);
Route::get('posts/{post}', [PublicPostController::class, 'show']);
Route::get('archives', [PublicPostController::class, 'archives']);
Route::get('archiveposts', [PublicPostController::class, 'archiveposts']);
Route::get('popular', [PublicPostController::class, 'popular']);
Route::get('facebook/facebook-rss', [PublicPostController::class, 'facebookShow']);
Route::get('posts/{post}/amp', [PublicPostController::class, 'ampShow']);
Route::get('page/{page}', [PublicPostController::class, 'showPage']);
Route::get('feed', [PublicPostController::class, 'feedControl']);
Route::get('search', [PublicPostController::class, 'search']);

Route::post('post/{id}/click', [LikeController::class, 'likePost']);

Route::get('categories', [PublicTagController::class, 'tags']);
Route::get('/category/{tag}', [PublicTagController::class, 'index']);

Route::get('adminprofile', [UserController::class, 'adminProfile']);
Route::put('adminprofile/{id}', [UserController::class, 'adminUpdate']);

Route::get('profile/{username}', [ProfileController::class, 'profile']);
Route::get('profile/{username}/edit', [ProfileController::class, 'edit']);
Route::put('profile/{id}', [ProfileController::class, 'update']);

Route::get('settings', [SettingController::class, 'index']);
Route::put('settings/{id}', [SettingController::class, 'update']);

Route::get('home', [HomeController::class, 'index'])->name('home');
Route::get('home/add', [HomeController::class, 'addpost']);
Route::post('delete/content', [HomeController::class, 'delcontent']);

Route::post('siteinstant', [InstantController::class, 'siteCheck']);
Route::get('deactivate', [InstantController::class, 'deactivatePage']);
Route::get('deactivation-result', [InstantController::class, 'deactivateResult']);
Route::post('deactivateinstant', [InstantController::class, 'deactivateScript']);

Route::post('admincp/uploadImg', [FileUploadController::class, 'postImage']);
Route::post('admincp/deleteImg', [FileUploadController::class, 'deleteFile']);

Route::post('admincp/postEmbed', [EmbedController::class, 'fetchEmbed']);
Route::post('admincp/deleteEmbed', [EmbedController::class, 'deleteEmbed']);

Route::post('cnt/multiple', [PostController::class, 'multiple']);
Route::get('unpublished', [PostController::class, 'unpublished']);

Route::resource('cats', TagController::class);
Route::resource('home', HomeController::class);
Route::resource('users', UserController::class);
Route::resource('pages', PageController::class);
Route::resource('contents', PostController::class);

Route::get('auth/{driver}', [LoginController::class, 'redirectToProvider']);
Route::get('auth/{driver}/callback', [LoginController::class, 'handleProviderCallback']);

Route::get('instant/clear', function() {

   Artisan::call('cache:clear');
   Artisan::call('config:clear');
   Artisan::call('config:cache');
   Artisan::call('view:clear');

   return "Cleared!";

});

require __DIR__.'/auth.php';
