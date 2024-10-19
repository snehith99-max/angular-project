import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnTraisepurchaseindentComponent } from './otl-trn-traisepurchaseindent.component';

describe('OtlTrnTraisepurchaseindentComponent', () => {
  let component: OtlTrnTraisepurchaseindentComponent;
  let fixture: ComponentFixture<OtlTrnTraisepurchaseindentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnTraisepurchaseindentComponent]
    });
    fixture = TestBed.createComponent(OtlTrnTraisepurchaseindentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
