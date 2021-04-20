<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Embed;
use GuzzleHttp\Client;
use Illuminate\Support\Str;
use Validator;

class EmbedController extends Controller
{
    public function fetchEmbed(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'embed' => 'required',
        ]);

        //Fetch Embed Code
        if ($validator->passes()) {
            $embed_url = $request->input('embed');
            $embed_type = $request->input('type');

            if ($embed_type == 'tweet') {
                $oembed_url = 'https://publish.twitter.com/oembed';
                $omit = 'omit_script';
                $attributes['short_url'] = class_basename($embed_url);
                $attributes['url'] = $embed_url;
            }

            if ($embed_type == 'facebook') {
                $client = new Client();
                try {
                $res = $client->request('GET', 'https://graph.facebook.com/oauth/access_token', [
                    'query' => [
                            'client_id' => config('services.facebook.client_id'),
                            'client_secret' => config('services.facebook.client_secret'),
                            'grant_type' => 'client_credentials'
                            ]
                ]);
                $data = json_decode($res->getBody(), true);
                $access_token =  $data['access_token'];
                } catch (\Exception $e) {
                    $e->getMessage();
                    return response()->json([
                        'error' => ['Your Facebook App Not Correct!']
                    ]);
                }

                $oembed_url = 'https://graph.facebook.com/v8.0/oembed_post';
                $omit = 'omitscript';
                $attributes['short_url'] = null;
                $attributes['url'] = $embed_url;
            }

            if ($embed_type == 'instagram') {
                $client = new Client();
                try {
                $res = $client->request('GET', 'https://graph.facebook.com/oauth/access_token', [
                    'query' => [
                            'client_id' => config('services.facebook.client_id'),
                            'client_secret' => config('services.facebook.client_secret'),
                            'grant_type' => 'client_credentials'
                            ]
                ]);
                $data = json_decode($res->getBody(), true);
                $access_token =  $data['access_token'];
                } catch (\Exception $e) {
                    $e->getMessage();
                    return response()->json([
                        'error' => ['Your Facebook App Not Correct!']
                    ]);
                }
                
                $oembed_url = 'https://graph.facebook.com/v8.0/instagram_oembed';
                $omit = 'omitscript';
                $inststr = substr($embed_url, strrpos($embed_url, 'p/') + 2);
                $attributes['short_url'] = strtok($inststr, '/');
                $attributes['url'] = $embed_url;
            }

            if ($embed_type == 'pinterest') {                
                $pin_id =  preg_replace('/[^0-9]/', '', $embed_url);
                $contains = Str::of($embed_url)->contains('pinterest.com');
                if ($contains) {
                    $attributes['short_url'] = $pin_id;
                    $attributes['url'] = $embed_url;                
                    $embed = Embed::create($attributes);
                    $embedID = $embed->id;
                    if ($embed) {
                        return response()->json(['success'=> $embedID]);
                    }

                } else {
                    return response()->json([
                        'error'=> [__('messages.form.nopin')]
                    ]);
                }
            }

            $client = new Client();
            try {
                if (!empty ($access_token)) {
                $result = $client->get($oembed_url, [
                    'query' => [
                        'url' => $embed_url,
                        'access_token' => $access_token,
                        $omit => 'true',
                    ]
                ]);
                } else {
                $result = $client->get($oembed_url, [
                    'query' => [
                        'url' => $embed_url,
                        $omit => 'true',
                    ]
                ]);
                }
                $data = json_decode($result->getBody(), true);
                $attributes['embedcode'] =  $data['html'];
            } catch (\Exception $e) {
                $e->getMessage();
                return response()->json([
                    'error'=> [__('messages.form.noembed')]
                ]);
            }

            $embed = Embed::create($attributes);
            $embedID = $embed->id;

            if ($embed) {
                return response()->json(['success'=> $embedID]);
            }
        }

        return response()->json(['error'=>$validator->errors()->all()]);
    }
}
