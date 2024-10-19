import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstWhatsappproductpricemanagementComponent } from './smr-mst-whatsappproductpricemanagement.component';

describe('SmrMstWhatsappproductpricemanagementComponent', () => {
  let component: SmrMstWhatsappproductpricemanagementComponent;
  let fixture: ComponentFixture<SmrMstWhatsappproductpricemanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstWhatsappproductpricemanagementComponent]
    });
    fixture = TestBed.createComponent(SmrMstWhatsappproductpricemanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
