import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { map } from 'rxjs';

export const successInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);
  return next(req).pipe(map((res: any) => {

    if (req.method !== 'POST' && req.method !== 'PUT' && req.method !== 'DELETE') {
      return res;
    }

    if (res?.body?.message === null || res?.body?.message === undefined) {
      return res;
    }

    snackBar.open(res.body.message, undefined, {panelClass: ['success-snackbar']});
    return res;
  }));
};
