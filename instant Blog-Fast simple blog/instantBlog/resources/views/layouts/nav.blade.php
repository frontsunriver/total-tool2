@if (!empty($setting->site_analytic))
    {!! $setting->site_analytic !!}
@endif
<nav class="navbar navbar-expand-md navbar-dark bg-nav">    
    <a class="navbar-brand" href="{{ url('/') }}">
        <img src="{{ url('/images/' . $setting->site_logo) }}" class="d-inline-block align-top" alt="">
    </a>
    <button class="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#navbarsDefault" aria-controls="navbarsDefault" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarsDefault">
        <ul class="navbar-nav ml-auto">
            <li class="nav-item {{ Request::is('/') ? 'active' : '' }}">
                <a class="nav-link mr-3 d-none d-md-block" href="{{ url('/') }}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.hometxt')"><i class="icon-home icons nav-icon"></i></a>
                <a class="nav-link d-md-none" href="{{ url('/') }}"><i class="icon-home icons"></i> @lang('messages.hometxt')</a>
            </li>
            <li class="nav-item {{ Request::is('popular') ? 'active' : '' }}">
                <a class="nav-link mr-3 d-none d-md-block" href="{{ url('/popular') }}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.populartxt')"><i class="icon-rocket icons nav-icon"></i></a>
                <a class="nav-link d-md-none" href="{{ url('/popular') }}"><i class="icon-rocket icons"></i> @lang('messages.populartxt')</a>
            </li>
            <li class="nav-item {{ Request::is('categories') ? 'active' : '' }}">
                <a class="nav-link mr-3 d-none d-md-block" href="{{ url('/categories') }}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.categoriestxt')"><i class="icon-grid icons nav-icon"></i></a>
                <a class="nav-link d-md-none" href="{{ url('/categories') }}"><i class="icon-grid icons"></i> @lang('messages.categoriestxt')</a>
            </li>
            <li class="nav-item {{ Request::is('archives') ? 'active' : '' }}">
                <a class="nav-link mr-3 d-none d-md-block" href="{{ url('/archives') }}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.archivestxt')"><i class="icon-clock icons nav-icon"></i></a>
                <a class="nav-link d-md-none" href="{{ url('/archives') }}"><i class="icon-clock icons"></i> @lang('messages.archivestxt')</a>
            </li>
            <li class="nav-item">
                <span data-toggle="modal" data-target="#searchModal">
                    <a class="nav-link mr-3 d-none d-md-block" href="#" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.searchtxt')"><i class="icon-magnifier icons nav-icon"></i></a>
                    <a class="nav-link d-md-none" href="#"><i class="icon-magnifier icons"></i> @lang('messages.searchtxt')</a>
                </span>
            </li>
            @unless (Auth::check())
            <li class="nav-item {{ Request::is('login') ? 'active' : '' }}">
                <a class="nav-link mr-3 d-none d-md-block" href="{{ url('/login') }}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.logintxt')"><i class="icon-user icons nav-icon"></i></a>
                <a class="nav-link d-md-none" href="{{ url('/login') }}"><i class="icon-user icons"></i> @lang('messages.logintxt')</a>
            </li>
            @else          
            @can('admin-area')
            <li class="nav-item dropdown">
                <a class="nav-link mr-3 dropdown-toggle" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <img class="avatar-xs img-fluid rounded-circle" src="{{ url('/images/' . Auth::user()->avatar) }}">
                    {{ Auth::user()->name }}
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                    <a class="dropdown-item" href="{{ url('/admin') }}">Admin Panel</a>
                    <a class="dropdown-item" href="{{ url('/home/add') }}">@lang('messages.addpost')</a>
                    <a class="dropdown-item" href="{{ url('/profile/' . Auth::user()->username) }}">@lang('messages.mpro')</a>
                    <a class="dropdown-item" href="{{ url('/logout') }}"
                    onclick="event.preventDefault();
                    document.getElementById('logout-form').submit();">
                    @lang('messages.logout')
                    </a>
                <form id="logout-form" action="{{ url('/logout') }}" method="POST" style="display: none;">
                    @csrf
                </form>
                </div>
            </li>
            @else
            <li class="nav-item dropdown">
                <a class="nav-link mr-3 dropdown-toggle" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                @if (substr( Auth::user()->avatar, 0, 4 ) === "http")
                <img class="avatar-xs img-fluid rounded-circle" src="{{ Auth::user()->avatar }}">
                @else
                <img class="avatar-xs img-fluid rounded-circle" src="{{ url('/images/' . Auth::user()->avatar) }}">
                @endif
                {{ Auth::user()->name }}
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                    <a class="dropdown-item" href="{{ url('/home') }}">@lang('messages.mpanel')</a>
                @if (auth()->user()->can('edit-post') OR $setting->allow_users == '0')
                    <a class="dropdown-item" href="{{ url('/home/add') }}">@lang('messages.addpost')</a>
                @endif
                    <a class="dropdown-item" href="{{ url('/profile/' . Auth::user()->username) }}">@lang('messages.mpro')</a>
                    <a class="dropdown-item" href="{{ url('/logout') }}"
                    onclick="event.preventDefault();
                    document.getElementById('logout-form').submit();">
                    @lang('messages.logout')
                    </a>
                    <form id="logout-form" action="{{ url('/logout') }}" method="POST" style="display: none;">
                        @csrf
                    </form>
                </div>
            </li>
            @endcan
            @endunless
        </ul>
    </div>
</nav>

<!-- Modal -->
<div class="modal fade" id="searchModal" tabindex="-1" aria-labelledby="searchModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content bg-dark">
            <div class="modal-body p-5">
                <div class="row">
                    <div class="col-10">                    
                        <form class="form-inline" action="{{ url('/search') }}" method="get">
                            <div class="input-group mb-3">
                                <input name="s" type="text" class="form-control form-control-lg" id="lgFormGroupInput" placeholder="@lang('messages.searchtxt')" aria-describedby="button-addon2" required>
                              <div class="input-group-append">
                                <button type="submit" id="button-addon2" class="btn btn-outline-secondary"><i class="icon-magnifier icons"></i></button>
                              </div>
                            </div>
                        </form>
                    </div>                      
                    <div class="col-2">
                        <button type="button" class="close mr-3" data-dismiss="modal" aria-label="Close">
                            <span class="text-white" aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
