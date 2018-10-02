import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ProductService } from '../../../services/product/product.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductDescription } from '../../../models/products/product-description';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  productForm: FormGroup;
  product: ProductDescription = new ProductDescription();

  formErrors = {
    'avgRating': '',
    'discription': ''
  };

  validationMessages = {
    'avgRating': {
      'required': 'Обязательное поле.',
      'min': 'Значение должно быть не менее 1.',
      'max': 'Значение не должно быть больше 5.'
      },
    'discription': {
      'required': 'Обязательное поле.',
    }
  };

constructor(private http: HttpClient, private productService: ProductService,
  private router: Router, private fb: FormBuilder) { }

  selectedFile: File = null;

  isHidden = false;

  enableAdd = true;

  buildForm() {
    this.productForm = this.fb.group({
      'avgRating': [this.product.Rating, [
        Validators.required,
        Validators.min(1),
        Validators.max(5)
      ]],
      'discription': [this.product.Description, [
        Validators.required
      ]]
    });

    this.productForm.valueChanges.subscribe(data => this.onValueChange(data));

    this.onValueChange();
  }

  onValueChange(data?: any) {
    if (!this.productForm) { return; }
    const form = this.productForm;

    // tslint:disable-next-line:forin
    for (const field in this.formErrors) {
        this.formErrors[field] = '';
        const control = form.get(field);

        if (control && control.dirty && !control.valid) {
            const message = this.validationMessages[field];
            // tslint:disable-next-line:forin
            for (const key in control.errors) {
                this.formErrors[field] += message[key] + ' ';
            }
        }
    }
  }

  onSubmit() {
    console.log('submitted');
    console.log(this.productForm.value);
}




onFileSelected(event) {
  this.selectedFile = <File>event.target.files[0];

  this.disabled();
}

onUpload(rating, descrText) {
  if (this.selectedFile !== null) {
    const fd = new FormData;
    fd.append('image', this.selectedFile, this.selectedFile.name);
    const url = `${environment.apiHostUrl}/api/products/`;

    this.http.post(url, fd)
    .subscribe(
      res => this.productService.addProductDescription(Number(rating), descrText, String(res)), // res is an ID of added product
      err => console.log(err));

      // Если продукт добавился div скрывается!
      this.router.navigate(['/products']);
  }
}

addProduct() {
  this.productService.addProductDescription(this.product.Rating, this.product.Description, '');
  this.router.navigate(['/products']);
}

  disabled(): void {
    const discription = document.getElementById('discription');
    discription.removeAttribute('disabled');
    const rat = document.getElementById('avgRating');
    rat.removeAttribute('disabled');
  }

  ngOnInit() {
    const discription = document.getElementById('discription');
    const rat = document.getElementById('avgRating');
    discription.attributes.setNamedItem(document.createAttribute('disabled'));
    rat.attributes.setNamedItem(document.createAttribute('disabled'));

    this.buildForm();
  }

  turnedOffWhile() {
  }

  hideBlockAdd() {
    this.router.navigate(['/products']);
  }
}
