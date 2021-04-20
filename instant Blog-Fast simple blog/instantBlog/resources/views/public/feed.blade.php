@php 
print '<?xml version="1.0" encoding="UTF-8" ?>'; 
@endphp
<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">
<channel>
<title>{{ $setting->site_name . ' - ' . $setting->site_title }}</title>
<description>RSS Feed</description>
<link>{{ url('/') }}</link>
<atom:link href="{{ url('/feed') }}" rel="self" type="application/atom+xml" />
@foreach ($posts as $post)
    @php
    $title = str_replace("&", "&amp;", $post->post_title);
    $title = stripslashes($post->post_title);
    if (!empty($post->post_desc)) {
      $description = str_replace("&rdquo;", "”", $post->post_desc);
      $description = str_replace("&ldquo;", "“", $post->post_desc);
      $pdescription = stripslashes($post->post_desc);
    } else {
      $description = null;
    }
    @endphp

    <item>
        <title>{{ $title }}</title>
        <description>{{ $description  }}</description>
        <pubDate>{{ date('D, d M Y H:i:s', strtotime($post->created_at)) }} GMT</pubDate>
        <link>{{ url('/posts/' . $post->post_slug ) }}</link>
        <guid>{{ url('/posts/' . $post->post_slug ) }}</guid>
    </item>
@endforeach
</channel>
</rss>