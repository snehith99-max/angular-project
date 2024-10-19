import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstProductAddComponent } from './otl-mst-product-add.component';

describe('OtlMstProductAddComponent', () => {
  let component: OtlMstProductAddComponent;
  let fixture: ComponentFixture<OtlMstProductAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstProductAddComponent]
    });
    fixture = TestBed.createComponent(OtlMstProductAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
