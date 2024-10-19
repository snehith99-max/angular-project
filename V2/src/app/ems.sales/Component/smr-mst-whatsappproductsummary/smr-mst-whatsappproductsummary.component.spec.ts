import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstWhatsappproductsummaryComponent } from './smr-mst-whatsappproductsummary.component';

describe('SmrMstWhatsappproductsummaryComponent', () => {
  let component: SmrMstWhatsappproductsummaryComponent;
  let fixture: ComponentFixture<SmrMstWhatsappproductsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstWhatsappproductsummaryComponent]
    });
    fixture = TestBed.createComponent(SmrMstWhatsappproductsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
