<!doctype html>
<html amp lang="en">
<head>
  <meta charset="utf-8">
  <script async src="https://cdn.ampproject.org/v0.js"></script>
  <title>{{ $post->post_title }}</title>
  <link rel="canonical" href="{{ url('posts/' . $post->post_slug) }}">
  <meta name="viewport" content="width=device-width,minimum-scale=1,initial-scale=1">
  <script async custom-element="amp-youtube" src="https://cdn.ampproject.org/v0/amp-youtube-0.1.js"></script>
  <script async custom-element="amp-twitter" src="https://cdn.ampproject.org/v0/amp-twitter-0.1.js"></script>
  <script async custom-element="amp-instagram" src="https://cdn.ampproject.org/v0/amp-instagram-0.1.js"></script>
  <script async custom-element="amp-facebook" src="https://cdn.ampproject.org/v0/amp-facebook-0.1.js"></script>
  <script async custom-element="amp-pinterest" src="https://cdn.ampproject.org/v0/amp-pinterest-0.1.js"></script>
  <script async custom-element="amp-social-share" src="https://cdn.ampproject.org/v0/amp-social-share-0.1.js"></script>
  @if ($setting->amp_ad_server == "1")
  <script async custom-element="amp-ad" src="https://cdn.ampproject.org/v0/amp-ad-0.1.js"></script>
  @endif
  <style amp-boilerplate>body{-webkit-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-moz-animation:-amp-start 8s steps(1,end) 0s 1 normal both;-ms-animation:-amp-start 8s steps(1,end) 0s 1 normal both;animation:-amp-start 8s steps(1,end) 0s 1 normal both}@-webkit-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-moz-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-ms-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@-o-keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}@keyframes -amp-start{from{visibility:hidden}to{visibility:visible}}</style><noscript><style amp-boilerplate>body{-webkit-animation:none;-moz-animation:none;-ms-animation:none;animation:none}</style></noscript>
  <style amp-custom>
  body {
    font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #212529;
    background-color: #fff;
  }
  .amp-ib-header {
    background: #252830;
  }
  .amp-ib-article {
    color: #353535;
    font-weight: 400;
    margin:0 auto;
    max-width: 840px;
    overflow-wrap: break-word;
    word-wrap: break-word;
  }
  .amp-ib-header div {
    color: #fff;
    font-size: 1em;
    font-weight: 400;
    margin: 0 auto;
    max-width: calc(840px - 32px);
    padding: .875em 16px;
    position: relative;
  }
  .amp-ib-related a{
    display: block;
    color: #000;
    background-color: #f5f5f5;
    text-decoration: none;
    font-size: 1em;
    font-weight: 500;
    margin: 1rem;
    padding: 15px;
    position: relative;
    box-shadow: 0 1px 1px 0 rgba(0,0,0,.14), 0 1px 1px -1px rgba(0,0,0,.14), 0 1px 5px 0 rgba(0,0,0,.12);
  }
  .amp-sm-text {
    font-size: 80%;
    font-weight: 400;
    color: #868e96;
  }
  .amp-ib-margin {
    margin-right: 1rem;
    margin-left: 1rem;
  }
  .amp-ib-share {
   text-align: center;
   padding: 10px;
   margin: 15px 0;
   border-top: 1px solid rgba(0,0,0,0.07);
   border-bottom: 1px solid rgba(0,0,0,0.07);
  }
  </style>
</head>
  <body>
    <header class="amp-ib-header">
      <div>
      <a href="{{ url('/') }}">
        <amp-img src="{{ url('/images/' . $setting->site_logo) }}" width="200" height="35"></amp-img>
      </a>
      </div>
    </header>    
    <article class="amp-ib-article">
      @if (!empty($post->post_video))
      <amp-youtube data-videoid="{{ $post->post_video }}"
      layout="responsive"
      width="480" height="270"></amp-youtube>
      @elseif(!empty($post->post_media))
      @php
      list($width, $height) = getimagesize(asset('/uploads/' . $post->post_media)); 
      @endphp
      <amp-img src="{{ url('/uploads/' . $post->post_media) }}" width="{{ $width }}" height="{{ $height }}" layout="responsive"></amp-img>
      @endif

      <h1 class="amp-ib-margin">{{ $post->post_title }}</h1>

      @if (!empty($post->post_desc))
      <p class="amp-ib-margin">{{ $post->post_desc }}</p>        
      @endif

      <p class="amp-ib-margin amp-sm-text">{{ $post->created_at }} - {{ $post->user->name }}</p>

      @if ($setting->amp_ad_server == "1")
      {!! $setting->amp_adscode !!}
      @endif

      @if($post->contents)
      @foreach ($post->contents as $content)
      @if ($content->type == "header")
      <h2 class="amp-ib-margin">{{ $content->body }}</h2>
      @endif

      @if ($content->type == "text")
      <p class="amp-ib-margin">{{ $content->body }}</p>
      @endif

      @if ($content->type == "txteditor")
      <div class="amp-ib-margin">
      {!! clean( $content->body ) !!}
      </div>     
      @endif

      @if ($content->type == "image")
      @php
      list($width, $height) = getimagesize(asset('/uploads/' . $content->body)); 
      @endphp
        <amp-img src="{{ url('/uploads/' . $content->body) }}" width="{{ $width }}" height="{{ $height }}" layout="responsive"></amp-img>
      @endif

      @if ($content->type == "youtube")
      <amp-youtube data-videoid="{{ $content->body }}"
      layout="responsive"
      width="480" height="270"></amp-youtube>
      @endif

      @if ($content->type == "tweet")
      <amp-twitter width="552"
        height="310"
        layout="responsive"
        data-tweetid="{{ $content->embed->short_url }}">
      </amp-twitter>
      @endif

      @if ($content->type == "facebook")
      <amp-facebook width="552"
        height="310"
        layout="responsive"
        data-href="{{ $content->embed->url }}">
      </amp-facebook>
      @endif

      @if ($content->type == "instagram")
      <amp-instagram data-shortcode="{{ $content->embed->short_url }}"
        width="552"
        height="310"
        layout="responsive">
      </amp-instagram>
      @endif

      @if ($content->type == "pinterest")
      <amp-pinterest 
        width="552"
        height="310"
        layout="responsive"
        data-do="embedPin"
        data-width="medium"
        data-url="{{ $content->embed->url }}">
      </amp-pinterest>
      @endif

      @endforeach
      @endif
      @if ($setting->amp_ad_server == "1")
      {!! $setting->amp_adscode !!}
      @endif
      <div class="amp-ib-share">
        <amp-social-share type="twitter"></amp-social-share>
        <amp-social-share type="facebook" data-param-app_id="{{ env('FACEBOOK_API_ID') }}"></amp-social-share>
        <amp-social-share type="gplus"></amp-social-share>
        <amp-social-share type="linkedin"></amp-social-share>
        <amp-social-share type="pinterest" data-param-url="{{ url('posts/' . $post->post_slug) }}"></amp-social-share>
      </div>
      @if (!empty($related))
        <h2 class="amp-ib-margin">@lang('messages.morepost')</h2>
        @foreach($related as $relatedpost)
            <div class="amp-ib-related">
            <a href="{{url('/posts/' . $relatedpost->post_slug . '/amp')}}">
              <span>{{ str_limit($relatedpost->post_title, 60) }}</span>
            </a>
            </div>
        @endforeach
      @endif
      
    </article>
  </body>
</html>