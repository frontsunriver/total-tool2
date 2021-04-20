@extends('vendor.installer.layouts.master')

@section('template_title')
    {{ trans('installer_messages.title') }}
@endsection

@section('title')
    <i class="fa fa-code fa-fw" aria-hidden="true"></i> {{ trans('installer_messages.environment.title') }}
@endsection

@section('container')

    <form method="post" action="{{ route('LaravelInstaller::environmentSaveClassic') }}">
        {!! csrf_field() !!}
        <textarea class="textarea" name="envConfig">{{ $envConfig }}</textarea>
        <div class="buttons buttons--right">
            <button class="button button--light" type="submit">
            	<i class="icon-settings icons" aria-hidden="true"></i>
             	<strong>{!! trans('installer_messages.environment.save') !!}</strong>
            </button>
        </div>
    </form>

    @if( ! isset($environment['errors']))
        <div class="buttons">
            <a class="button" href="{{ route('LaravelInstaller::database') }}">
                <i class="fa fa-check fa-fw" aria-hidden="true"></i>
                {!! trans('installer_messages.environment.install') !!}
                <i class="fa fa-angle-double-right fa-fw" aria-hidden="true"></i>
            </a>
        </div>
    @endif

@endsection