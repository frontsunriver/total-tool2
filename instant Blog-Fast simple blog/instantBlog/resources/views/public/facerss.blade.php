<rss version="2.0" xmlns:content="http://purl.org/rss/1.0/modules/content/">
  <channel>
    <title>{{ $setting->site_name . ' - ' . $setting->site_title }}</title>
    <link>{{ url('/') }}</link>
    <description>Facebook RSS Feed</description>
    <language>en-us</language>
    <lastBuildDate>{{ date('c') }}</lastBuildDate>
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
        <author>{{ $post->user->username }}</author>
        <content:encoded>
          <![CDATA[
             @include('public.showfacebook')
          ]]>
        </content:encoded>
      </item>
    @endforeach
  </channel>
</rss>