<div class="d-none">
    <div id="header_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.heading')</label>
            <div class="col-sm-10 col-md-7">
                <input type="text" class="form-control" name="content[]" placeholder="@lang('messages.form.heading_help')">
                <input type="hidden" name="type[]" value="header">
            </div>
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="txt_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.text')</label>
            <div class="col-sm-10 col-md-7">
                <textarea class="form-control" name="content[]" placeholder="@lang('messages.form.text_help')"></textarea>
                <input type="hidden" name="type[]" value="text">
            </div>
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="img_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.upload')</label>
            <div class="col-sm-4 col-md-2 fileinputs">
                <label class="btn btn-info btn-block btnfile">@lang('messages.form.browse')
                    <input class="fileupload d-none" type="file" name="post_image">
                </label>
            </div>
            <input class="photo_upload" name="content[]" type="hidden" value="">               
            <input type="hidden" name="type[]" value="image">
            <div class="col-sm-6 col-md-5 fileinfo">
            </div>
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="youtube_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.youtube')</label>
            <div class="col-sm-10 col-md-7">
                <input type="text" class="form-control" name="content[]" placeholder="@lang('messages.form.youtube_help')">
                <input type="hidden" name="type[]" value="youtube">
            </div>
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="tweet_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.tweet')</label>
            <div class="input-group col-sm-10 col-md-7">                
                <input name="embed_url" type="text" class="form-control" placeholder="@lang('messages.form.tweet_help')">
                <span class="input-group-btn">
                    <button type="button" class="btn btn-success embed_btn">@lang('messages.form.embed')</button>
                </span>
            </div>
            <input type="hidden" class="form-control" name="content[]" value="">
            <input type="hidden" name="type[]" value="tweet">
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="face_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.facebook')</label>
            <div class="input-group col-sm-10 col-md-7">                
                <input name="embed_url" type="text" class="form-control" placeholder="@lang('messages.form.facebook_help')">
                <span class="input-group-btn">
                    <button type="button" class="btn btn-success embed_btn">@lang('messages.form.embed')</button>
                </span>
            </div>
            <input type="hidden" class="form-control" name="content[]" value="">
            <input type="hidden" name="type[]" value="facebook">
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="instagram_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.instagram')</label>
            <div class="input-group col-sm-10 col-md-7">                
                <input name="embed_url" type="text" class="form-control" placeholder="@lang('messages.form.instagram_help')">
                <span class="input-group-btn">
                    <button type="button" class="btn btn-success embed_btn">@lang('messages.form.embed')</button>
                </span>
            </div>
            <input type="hidden" class="form-control" name="content[]" value="">
            <input type="hidden" name="type[]" value="instagram">
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
    <div id="pinterest_add">
        <div class="form-group row">
            <label class="offset-md-1 col-md-2 col-form-label">@lang('messages.form.pinterest')</label>
            <div class="input-group col-sm-10 col-md-7">                
                <input name="embed_url" type="text" class="form-control" placeholder="@lang('messages.form.pinterest_help')">
                <span class="input-group-btn">
                    <button type="button" class="btn btn-success embed_btn">@lang('messages.form.embed')</button>
                </span>
            </div>
            <input type="hidden" class="form-control" name="content[]" value="">
            <input type="hidden" name="type[]" value="pinterest">
            <div class="col-sm-2 col-md-1">
                <button type="button" class="btn btn-danger btn_remove">X</button>
            </div>
        </div>
    </div>
</div>