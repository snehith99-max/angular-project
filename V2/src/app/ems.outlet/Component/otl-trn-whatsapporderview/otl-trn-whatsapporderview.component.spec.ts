import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnWhatsapporderviewComponent } from './otl-trn-whatsapporderview.component';

describe('OtlTrnWhatsapporderviewComponent', () => {
  let component: OtlTrnWhatsapporderviewComponent;
  let fixture: ComponentFixture<OtlTrnWhatsapporderviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnWhatsapporderviewComponent]
    });
    fixture = TestBed.createComponent(OtlTrnWhatsapporderviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
