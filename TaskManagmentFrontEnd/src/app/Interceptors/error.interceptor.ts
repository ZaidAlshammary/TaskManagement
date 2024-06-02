import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);
  return next(req).pipe(catchError(err => {
      console.error(err.error);
      
      let message = "An error occurred";

      if (err.error && err.error.message) {
        message = err.error.message;
      }

      snackBar.open(message, undefined, {panelClass: ['error-snackbar']});
      
      return throwError(() => new Error(message));
    }
  ));
};
