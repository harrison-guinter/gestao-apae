import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
  LOCALE_ID,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

import { routes } from './app.routes';
import { ErrorInterceptor } from './modules/core/interceptors/error.interceptor';
import { ResponseInterceptor } from './modules/core/interceptors/response.interceptor';
import { RequestInterceptor } from './modules/core/interceptors/request.interceptor';
import { LoadingInterceptor } from './modules/core/interceptors/loading.interceptor';

// Registrar locale português
registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    importProvidersFrom(
      HttpClientModule,
      ReactiveFormsModule,
      MatSnackBarModule,
      BrowserAnimationsModule
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
  ],
};
