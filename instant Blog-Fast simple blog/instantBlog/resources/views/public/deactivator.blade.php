@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('jumbotron')
<div class="jumbotron bg-none">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-12">
                <h1 class="display-4 text-white">Deactivation</h1>
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="container mt-5">
    <div class="row">
        <div class="col-md-12">     
            <div class="card">
                <div class="card-body mb-5">
                    <h3>Deactivation</h3>                    
                    @if(!empty($setting->site_activation))
                    <p> If you want to deactivate the Instant Blog Script press below button. <br>After deactivation you need to activate the script for using again. Please read documenatiton for more information.</p>
                    <form class="form-horizontal" role="form" method="POST" action="{{ url('/deactivateinstant') }}">
                        <input type="hidden" name="_token" value="{{ csrf_token() }}">
                        <input type="hidden" name="id" value="1">
                        <button type="submit" class="btn btn-primary btnpoint">
                            Deactivate
                        </button>
                    </form>
                    @else
                    <p> You have already deactivated the script.</p>
                    @endif
                </div>
            </div>
        </div>
    </div>
</div>
@endsection