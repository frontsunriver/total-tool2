<!doctype html>
<html lang="en" prefix="op: http://media.facebook.com/op#">
  <head>
    <meta charset="utf-8">
    <link rel="canonical" href="{{ url('posts/' . $post->post_slug) }}">
    <meta property="op:markup_version" content="v1.0">
    <meta property="fb:article_style" content="{{ $setting->fb_theme }}">
    @if (!empty($setting->fb_ads_code))
    <meta property="fb:use_automatic_ad_placement" content="enable=true ad_density=default">
    @endif
  </head>
  <body>
    <article>
      <header>
        @if (!empty($setting->fb_ads_code))
        {!! $setting->fb_ads_code !!}
        @endif
        @if(!empty($post->post_media))
        <figure>
          <img src="{{ url('/uploads/' . $post->post_media) }}">
        </figure>
        @endif
        <h1>{{ $post->post_title }}</h1>

        @if (!empty($post->post_desc))
        <h2>{{ $post->post_desc }}</h2>        
        @endif

        <time class="op-published" datetime="{{ $post->created_at }}">{{ $post->created_at }}</time>

        <address>
          {{ $post->user->username }}
        </address>
      </header>
      @if (!empty($post->post_video))
        <figure class="op-interactive">
          <iframe class="no-margin" width="560" height="315" src="https://www.youtube.com/embed/{{ $post->post_video }}"></iframe>
        </figure>
      @endif
      @if($post->contents)
      @foreach ($post->contents as $content)
      @if ($content->type == "header")
      <h2>{{ $content->body }}</h2>
      @endif

      @if ($content->type == "text")
      <p>{{ $content->body }}</p>
      @endif

      @if ($content->type == "txteditor")
      {!! clean( $content->body ) !!}
      @endif

      @if ($content->type == "image")
      <figure>
        <img src="{{ url('/uploads/' . $content->body) }}">
      </figure>
      @endif

      @if ($content->type == "youtube")
      <figure class="op-interactive">
        <iframe width="560" height="315" src="https://www.youtube.com/embed/{{ $content->body }}"></iframe>
      </figure>
      @endif

      @if ($content->type == "tweet")
      <figure class="op-interactive">
        <iframe>
            {!! $content->embed->embedcode !!}
        </iframe>
      </figure>
      @endif

      @if ($content->type == "facebook")
      <figure class="op-interactive">
        <iframe>
        <div id="fb-root"></div>
        <script>(function(d, s, id) {
          var js, fjs = d.getElementsByTagName(s)[0];
          if (d.getElementById(id)) return;
          js = d.createElement(s); js.id = id;
          js.src = "https://connect.facebook.net/en_US/sdk.js#xfbml=1&amp;version=v2.5";
          fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));</script>
        {!! $content->embed->embedcode !!}
        </iframe>
      </figure>
      @endif

      @if ($content->type == "instagram")
      <figure class="op-interactive">
        <iframe>
            {!! $content->embed->embedcode !!}
        </iframe>
      </figure>
      @endif

      @if ($content->type == "pinterest")
      <figure class="op-interactive">
        <iframe>
         <a data-pin-do="embedPin" data-pin-width="medium" href="{{ $content->embed->url }}"></a>
         <script async defer src="//assets.pinterest.com/js/pinit.js"></script>
        </iframe>
      </figure>
      @endif

      @endforeach
      @endif

      @if (!empty($related))
      <footer>
       <ul class="op-related-articles">
        @foreach($related as $relatedpost)
            <li><a href="{{ url('/posts/' . $relatedpost->post_slug) }}"></a></li>
        @endforeach
        </ul>
      </footer>
      @endif
    </article>
  </body>
</html>