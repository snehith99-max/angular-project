import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstProductEditComponent } from './otl-mst-product-edit.component';

describe('OtlMstProductEditComponent', () => {
  let component: OtlMstProductEditComponent;
  let fixture: ComponentFixture<OtlMstProductEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstProductEditComponent]
    });
    fixture = TestBed.createComponent(OtlMstProductEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
