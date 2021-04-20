@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4 text-white">Activate Instant Blog</h1>
            </div>
        </div>
    </div>
</div>
@endsection

@section('content')
<div class="container text-white">
    <div class="row">
        <div class="col-md-9">
            <form class="form-horizontal" role="form" method="POST" action="{{ url('/siteinstant') }}">
                <input type="hidden" name="_token" value="{{ csrf_token() }}">
                <div class="form-group">
                    <label for="p_code">Enter Code Canyon Item Purchase Code :</label>
                    <input id="p_code" type="text" class="form-control" name="p_code" required autofocus>
                    <small id="p-codeHelp" class="form-text text-muted">Example : XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</small>
                </div>
                <div class="form-group">                    
                    <button type="submit" class="btn btn-primary btnpoint">
                        Activate
                    </button>
                </div>
                <div class="form-group">
                    <small id="passwordHelpBlock" class="form-text text-muted">
                    If you need more information about activating Instant Blog, please check documentation/readme file inside of downloaded zip file.
                    </small>
                </div>
            </form>
        </div>
    </div>
</div>
@endsection