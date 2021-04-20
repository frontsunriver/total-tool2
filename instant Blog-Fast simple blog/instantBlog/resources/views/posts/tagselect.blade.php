<div class="form-group row">
    <label for="post_select" class="offset-md-1 col-sm-2 col-form-label">
        @lang('messages.form.category')
    </label>
    <div class="col-sm-7">
        <select class="custom-select" name="tag_id">
            <option value="">@lang('messages.form.select_category')</option>
            @foreach($tags as $tag)
            <option value="{{ $tag->id }}" 
                @if (isset($postTag))
                {{  $postTag->pivot->tag_id == $tag->id ? 'selected="selected"' : '' }}
                @endif
                >{{ $tag->name }}</option>
            @endforeach
        </select>
    </div>
</div>