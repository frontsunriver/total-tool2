@if(!Request::is('store/*'))
<footer id="footer">
    <div class="footer-area pt-70">
        <div class="footer-main-content text-center">
            <div class="footer-menu">
                <nav>
                    <ul>
                        <li>
                            <h4><a href="{{ url('/') }}">{{ __('Home') }}</a></h4>
                        </li>
                        <li>
                            <h4><a href="{{ url('/contact') }}">{{ __('Contact') }}</a></h4>
                        </li>
                        <li>
                            <h4><a href="{{ url('/page/privacy-policy') }}">{{ __('Privacy Policy') }}</a></h4>
                        </li>
                        <li>
                            <h4><a href="{{ url('/page/terms-and-conditions') }}">{{ __('Terms & Conditions') }}</a></h4>
                        </li>
                        <li>
                            <h4><a href="{{ url('/page/refund-return-policy') }}">{{ __('Refund & Return Policy') }}</a></h4>
                        </li>
                    </ul>
                </nav>
            </div>
            <div class="footer-copyright">
                <p id="copyright_area">{{ content('footer','copyright_area') }}</p>
            </div>
        </div>
    </div>
</footer>
@endif