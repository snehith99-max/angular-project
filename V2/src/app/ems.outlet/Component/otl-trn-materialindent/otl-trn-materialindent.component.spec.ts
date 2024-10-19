import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnMaterialindentComponent } from './otl-trn-materialindent.component';

describe('OtlTrnMaterialindentComponent', () => {
  let component: OtlTrnMaterialindentComponent;
  let fixture: ComponentFixture<OtlTrnMaterialindentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnMaterialindentComponent]
    });
    fixture = TestBed.createComponent(OtlTrnMaterialindentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
