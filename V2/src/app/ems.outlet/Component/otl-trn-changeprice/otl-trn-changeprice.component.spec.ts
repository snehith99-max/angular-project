import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnChangepriceComponent } from './otl-trn-changeprice.component';

describe('OtlTrnChangepriceComponent', () => {
  let component: OtlTrnChangepriceComponent;
  let fixture: ComponentFixture<OtlTrnChangepriceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnChangepriceComponent]
    });
    fixture = TestBed.createComponent(OtlTrnChangepriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
