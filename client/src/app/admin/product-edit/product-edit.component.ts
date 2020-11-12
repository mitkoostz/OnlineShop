import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IProduct } from 'src/app/shared/models/product';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  baseUrl = environment.apiUrl;
  productForm: FormGroup;
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  productType: string;
  productGenderBase: string;
  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder,private cd: ChangeDetectorRef,private http: HttpClient,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.createEditProductForm();
  }
  createEditProductForm(){
    this.productForm = this.fb.group({
      id: new FormControl(''),
      name: new FormControl(''),
      description: new FormControl(''),
      price: new FormControl(''),
      productImage: new FormControl(''),
      productType: new FormControl(''),
      productGenderBase: new FormControl('')
  });
  this.productForm.patchValue({
       id: this.id,
       name: this.name,
       description: this.description,
       price: this.price,
       productType: this.productType,
       productGenderBase: this.productGenderBase
  });
  }

  onFileChange(event) {
    const reader = new FileReader();
 
    if(event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);
  
      reader.onload = () => {
        this.productForm.patchValue({
          productImage: reader.result
       });
      
        // need to run CD since file load runs outside of zone
        this.cd.markForCheck();
        this.pictureUrl = reader.result.toString(); 
        this.productForm.get('productImage').setValue(file);

      };
    }
  }
  onSubmit(){
    var formData: any = new FormData();
    formData.append("id", this.productForm.get('id').value);
    formData.append("name", this.productForm.get('name').value);
    formData.append("description", this.productForm.get('description').value);
    formData.append("price", this.productForm.get('price').value);
    formData.append("productImage", this.productForm.get('productImage').value);
    formData.append("productType", this.productForm.get('productType').value);
    formData.append("productGenderBase", this.productForm.get('productGenderBase').value);
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');

    this.http.put(this.baseUrl + "admin/updateproduct", formData , {headers: headers}).subscribe(
      (response) => this.toastr.success("Product is successfully updated!") ,
      (error) => this.toastr.error(error)
    )
  }

}
