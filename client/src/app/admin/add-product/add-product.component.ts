import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { isEmpty } from 'rxjs/operators';
import { IProductToAdd } from 'src/app/shared/models/productToAdd';
import { environment } from 'src/environments/environment';
import { AdminServiceService } from '../admin-service.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {
productForm: FormGroup;
imgUrl = "assets/images/StarsFashionHat422.png";
previewName = "Hat";
previewPrice =  19.99;
previewDesc = "Product description.";
baseUrl = environment.apiUrl;

  constructor( private fb: FormBuilder,  private cd: ChangeDetectorRef, private adminService: AdminServiceService,private http: HttpClient) { }

   gender = "Men";
  ngOnInit(): void {
    this.createAddProductForm();
  }

  createAddProductForm(){
    this.productForm = this.fb.group({
      name: new FormControl('',Validators.required),
      description: new FormControl('',Validators.required),
      price: new FormControl('',Validators.required),
      productImage: new FormControl('',Validators.required),
      productType: new FormControl('',Validators.required),
      productGenderBase: new FormControl('',Validators.required)
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
        this.imgUrl = reader.result.toString(); 
        this.productForm.get('productImage').setValue(file);

      };
    }
  }
  nameChanged(value: string){
    this.previewName = value;  
  }
  priceChanged(value: number){
    this.previewPrice = value;
  }
  descriptionChanged(value: string){
    this.previewDesc = value;
  }
  
 
  onSubmit(){
    var formData: any = new FormData();
    formData.append("name", this.productForm.get('name').value);
    formData.append("description", this.productForm.get('description').value);
    formData.append("price", this.productForm.get('price').value);
    formData.append("ProductImage", this.productForm.get('productImage').value);
    formData.append("productType", this.productForm.get('productType').value);
    formData.append("productGenderBase", this.productForm.get('productGenderBase').value);

       
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    
    console.log(this.productForm.value);
    console.log(this.productForm.get('productImage').value);
    this.http.put(this.baseUrl + "admin/addnewproduct", formData , {headers: headers}).subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    )

  }

}
