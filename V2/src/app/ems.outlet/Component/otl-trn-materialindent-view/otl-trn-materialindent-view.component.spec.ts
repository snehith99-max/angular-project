import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnMaterialindentViewComponent } from './otl-trn-materialindent-view.component';

describe('OtlTrnMaterialindentViewComponent', () => {
  let component: OtlTrnMaterialindentViewComponent;
  let fixture: ComponentFixture<OtlTrnMaterialindentViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnMaterialindentViewComponent]
    });
    fixture = TestBed.createComponent(OtlTrnMaterialindentViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
