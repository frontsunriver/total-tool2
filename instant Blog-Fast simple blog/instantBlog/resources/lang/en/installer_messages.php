<?php

return [

    /**
     *
     * Shared translations.
     *
     */
    'title' => 'Instant Blog Installer',
    'next' => 'Next Step',
    'finish' => 'Install',


    /**
     *
     * Home page translations.
     *
     */
    'welcome' => [
        'title'   => 'Instant Blog Installer',
        'message' => 'Welcome to the setup wizard.',
        'next' => 'Next Step',
    ],


    /**
     *
     * Requirements page translations.
     *
     */
    'requirements' => [
        'title' => 'Requirements',
        'next' => 'Next Step',
    ],


    /**
     *
     * Permissions page translations.
     *
     */
    'permissions' => [
        'title' => 'Permissions',
    ],


    /**
     *
     * Environment page translations.
     *
     */
    'environment' => [
        'title' => 'Environment Settings',
        'save' => 'Save .env',
        'success' => 'Your .env file settings have been saved.',
        'errors' => 'Unable to save the .env file, Please create it manually.',
        'install' => 'Install Now',

    ],

    /**
     *
     * Final page translations.
     *
     */
    'final' => [
        'title' => 'Finished',
        'finished' => 'Application has been successfully installed.',
        'exit' => 'Click here to exit',
    ],

    /**
     *
     * Update specific translations
     *
     */
    'updater' => [
        /**
         *
         * Shared translations.
         *
         */
        'title' => 'Laravel Updater',

        /**
         *
         * Welcome page translations for update feature.
         *
         */
        'welcome' => [
            'title'   => 'Welcome To The Updater',
            'message' => 'Welcome to the update wizard.',
        ],

        /**
         *
         * Welcome page translations for update feature.
         *
         */
        'overview' => [
            'title'   => 'Overview',
            'message' => 'There is 1 update.|There are :number updates.',
            'install_updates' => 'Install updates',
        ],

        /**
         *
         * Final page translations.
         *
         */
        'final' => [
            'title' => 'Finished',
            'finished' => 'Application\'s database has been successfully updated.',
            'exit' => 'Click here to exit',
        ],
    ],

];
