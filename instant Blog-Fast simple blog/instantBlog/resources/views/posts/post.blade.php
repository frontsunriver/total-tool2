<tr>
    <td><input type="checkbox" name="multiple[]" class="checkboxes" value="{{ $post->id }}" /></td>
    <td>
        <a href="{{url('/posts/' . $post->post_slug)}}">
            @if (! empty($post->post_title))
            {{ str_limit($post->post_title, 35) }}
            @else
            Title not found!
            @endif
        </a>
    </td>
    <td><a href="{{ url('/profile/' . $post->user->username) }}">{{ str_limit($post->user->name, 10) }}</a></td> 
    <td><a href="{{ url('/home/' . $post->id . '/edit') }}">Edit</a></td>
    <td><a class="color-delete" href="{{ url('/home/' . $post->id) }}">Delete</a></td>
</tr>