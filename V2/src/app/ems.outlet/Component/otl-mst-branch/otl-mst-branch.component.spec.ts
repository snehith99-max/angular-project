import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstBranchComponent } from './otl-mst-branch.component';

describe('OtlMstBranchComponent', () => {
  let component: OtlMstBranchComponent;
  let fixture: ComponentFixture<OtlMstBranchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstBranchComponent]
    });
    fixture = TestBed.createComponent(OtlMstBranchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
