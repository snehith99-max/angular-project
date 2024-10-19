import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnWhatsapporderComponent } from './otl-trn-whatsapporder.component';

describe('OtlTrnWhatsapporderComponent', () => {
  let component: OtlTrnWhatsapporderComponent;
  let fixture: ComponentFixture<OtlTrnWhatsapporderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnWhatsapporderComponent]
    });
    fixture = TestBed.createComponent(OtlTrnWhatsapporderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
