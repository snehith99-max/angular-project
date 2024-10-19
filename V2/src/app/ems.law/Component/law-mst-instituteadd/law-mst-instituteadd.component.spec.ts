import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstInstituteaddComponent } from './law-mst-instituteadd.component';

describe('LawMstInstituteaddComponent', () => {
  let component: LawMstInstituteaddComponent;
  let fixture: ComponentFixture<LawMstInstituteaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstInstituteaddComponent]
    });
    fixture = TestBed.createComponent(LawMstInstituteaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
